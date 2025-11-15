import { Component, inject, signal, OnInit, DestroyRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTabsModule } from '@angular/material/tabs';
import { finalize } from 'rxjs';

import { NotificationService } from '../../../../shared/services/notification.service';
import { PlansApiService } from '../../services/plans-api.service';
import { Plan, PlanDecision } from '../../models/plan.model';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { PlanDetailDialogData, PlanDetailDialogResult } from '../../models/plan-dialog.model';

@Component({
  selector: 'app-plan-detail-dialog',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatTabsModule
  ],
  templateUrl: './plan-detail-dialog.html',
  styleUrl: './plan-detail-dialog.scss'
})
export class PlanDetailDialog implements OnInit {
  private dialogRef = inject(MatDialogRef<PlanDetailDialog>);
  private dialogData = inject<PlanDetailDialogData>(MAT_DIALOG_DATA);
  private formBuilder = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);
  private plansApiService = inject(PlansApiService);
  private notificationService = inject(NotificationService);

  plan = signal<Plan>(this.dialogData.plan);
  isEditMode = signal<boolean>(false);
  isLoading = signal<boolean>(false);
  contentForm!: FormGroup;
  rejectReasonForm!: FormGroup;
  showRejectForm = signal<boolean>(false);

  ngOnInit(): void {
    this.initForms();
  }

  private initForms(): void {
    this.contentForm = this.formBuilder.group({
      contentMd: [this.plan().contentMd || '', Validators.required]
    });

    this.rejectReasonForm = this.formBuilder.group({
      decisionReason: ['', Validators.required]
    });

    if (!this.isEditMode()) {
      this.contentForm.disable();
    }
  }

  toggleEditMode(): void {
    if (this.isEditMode()) {
      this.contentForm.disable();
      this.isEditMode.set(false);
    } else {
      this.contentForm.enable();
      this.isEditMode.set(true);
    }
  }

  onAccept(): void {
    this.isLoading.set(true);

    const decision: PlanDecision = { decisionReason: '' };

    this.plansApiService.acceptPlan(this.plan().id, decision)
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => this.isLoading.set(false))
      )
      .subscribe({
        next: () => {
          this.notificationService.notifySuccess('Plan accepted successfully!');
          this.dialogRef.close({ action: 'accept' } as PlanDetailDialogResult);
        },
        error: (error) => this.notificationService.notifyError('Failed to accept plan.'),
      });
  }

  onReject(): void {
    if (this.showRejectForm()) {
      if (this.rejectReasonForm.invalid) {
        this.rejectReasonForm.markAllAsTouched();
        return;
      }

      this.isLoading.set(true);

      const decision: PlanDecision = {
        decisionReason: this.rejectReasonForm.value.decisionReason
      };

      this.plansApiService.rejectPlan(this.plan().id, decision)
        .pipe(
          takeUntilDestroyed(this.destroyRef),
          finalize(() => this.isLoading.set(false))
        )
        .subscribe({
          next: () => {
            this.notificationService.notifySuccess('Plan rejected.');
            this.dialogRef.close({ action: 'reject' } as PlanDetailDialogResult);
          },
          error: (error) =>  this.notificationService.notifyError('Failed to reject plan.'),
        });
    } else {
      this.showRejectForm.set(true);
    }
  }

  onUpdate(): void {
    if (this.contentForm.invalid) {
      this.contentForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);

    this.plansApiService.updatePlan(this.plan().id, { contentMd: this.contentForm.value.contentMd })
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => this.isLoading.set(false))
      )
      .subscribe({
        next: () => {
          this.notificationService.notifySuccess('Plan updated successfully!');
          this.dialogRef.close({ action: 'update' } as PlanDetailDialogResult);
        },
        error: (error) => {
          this.isLoading.set(false);
          this.notificationService.notifyError(error?.error?.message || 'Failed to update plan.');
        }
      });
  }

  onClose(): void {
    this.dialogRef.close({ action: 'close' } as PlanDetailDialogResult);
  }

  cancelReject(): void {
    this.showRejectForm.set(false);
    this.rejectReasonForm.reset();
  }

  isGenerated(): boolean {
    return this.plan().decisionStatus === 'generated';
  }

  isAccepted(): boolean {
    return this.plan().decisionStatus === 'accepted';
  }
}
