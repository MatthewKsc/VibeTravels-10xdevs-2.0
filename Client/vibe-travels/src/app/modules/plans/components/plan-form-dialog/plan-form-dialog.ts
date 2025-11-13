import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';

import { PlansApiService } from '../../services/plans-api.service';
import { UserProfileApiService } from '../../../user/services/user-profile-api.service';
import { PlanFormData } from '../../models/plan.model';
import { NotificationService } from '../../../../shared/services/notification.service';

export interface PlanFormDialogData {
  noteId: string;
}

@Component({
  selector: 'app-plan-form-dialog',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatIconModule,
    RouterLink
  ],
  templateUrl: './plan-form-dialog.html',
  styleUrl: './plan-form-dialog.scss'
})
export class PlanFormDialog implements OnInit {
  private dialogRef = inject(MatDialogRef<PlanFormDialog>);
  private dialogData = inject<PlanFormDialogData>(MAT_DIALOG_DATA);
  private formBuilder = inject(FormBuilder);
  private plansApiService = inject(PlansApiService);
  private userProfileApiService = inject(UserProfileApiService);
  private notificationService = inject(NotificationService);

  planForm!: FormGroup;
  isLoading = signal(false);
  isProfileIncomplete = signal(false);

  ngOnInit(): void {
    this.initForm();
    this.checkProfileCompletion();
  }

  private initForm(): void {
    this.planForm = this.formBuilder.group({
      travelDays: ['', [Validators.required, Validators.min(1), Validators.pattern('^[0-9]+$')]],
      travelers: ['', [Validators.required, Validators.min(1), Validators.pattern('^[0-9]+$')]],
      startDate: [new Date(), Validators.required]
    });
  }

  private checkProfileCompletion(): void {
    this.userProfileApiService.getUserProfile().subscribe({
      next: (profile) => {
        if (!profile || !profile.travelStyle || !profile.accommodationPreference || !profile.climatePreference) {
          this.isProfileIncomplete.set(true);
        }
      },
      error: () => {
        this.isProfileIncomplete.set(true);
      }
    });
  }

  onSubmit(): void {
    if (this.planForm.invalid) {
      this.planForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);

    const formData: PlanFormData = {
      noteId: this.dialogData.noteId,
      travelDays: Number(this.planForm.value.travelDays),
      travelers: Number(this.planForm.value.travelers),
      startDate: this.planForm.value.startDate
    };

    this.plansApiService.addPlan(formData).subscribe({
      next: () => {
        this.notificationService.notifySuccess('Plan generation started successfully!');
        this.dialogRef.close({ success: true });
      },
      error: (error) => {
        this.isLoading.set(false);
        this.notificationService.notifyError(error?.error?.message || 'Failed to generate plan. Please try again.');
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close({ success: false });
  }
}
