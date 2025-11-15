import { Component, DestroyRef, inject, signal, WritableSignal, OnInit, OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { finalize } from 'rxjs';
import { PlansToolbar } from '../plans-toolbar/plans-toolbar';
import { PlansList } from '../plans-list/plans-list';
import { PlansApiService } from '../../services/plans-api.service';
import { Plan } from '../../models/plan.model';
import { NotificationService } from '../../../../shared/services';
import { ConfirmationDialog } from '../../../../shared/components';
import { ConfirmationDialogData } from '../../../../shared/models';
import { PlanDetailDialog } from '../plan-detail-dialog/plan-detail-dialog';
import { PlanDetailDialogData, PlanDetailDialogResult } from '../../models/plan-dialog.model';

@Component({
  selector: 'app-plans-root',
  imports: [PlansToolbar, PlansList],
  templateUrl: './plans-root.html',
  styleUrl: './plans-root.scss',
})
export class PlansRoot implements OnInit, OnDestroy {
  private apiService = inject(PlansApiService);
  private destroyRef = inject(DestroyRef);
  private notificationService = inject(NotificationService);
  private dialog = inject(MatDialog);
  private refreshInterval?: number;

  plans: WritableSignal<Plan[] | undefined> = signal<Plan[] | undefined>(undefined);
  loadingData: WritableSignal<boolean> = signal<boolean>(false);

  private readonly REFRESH_INTERVAL_MS = 15000;

  ngOnInit(): void {
    this.fetchPlans();
    this.startAutoRefresh();
  }

  ngOnDestroy(): void {
    this.stopAutoRefresh();
  }

  private startAutoRefresh(): void {
    this.refreshInterval = window.setInterval(() => {
      this.fetchPlans();
    }, this.REFRESH_INTERVAL_MS);
  }

  private stopAutoRefresh(): void {
    if (this.refreshInterval) {
      clearInterval(this.refreshInterval);
    }
  }

  fetchPlans(): void {
    this.loadingData.set(true);

    this.apiService.getPlans()
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => this.loadingData.set(false))
      )
      .subscribe({
        next: (plans: Plan[]) => this.plans.set(plans),
        error: (error: any) => this.notificationService.notifyError('Failed to load plans. Please try again.'),
      });
  }

  onViewPlan(planId: string): void {
    const plan = this.getPlanById(planId);

    if (!plan) {
      this.notificationService.notifyWarning('Plan not found, refresh the page and try again.');
      return;
    }

    const mode = plan.decisionStatus === 'accepted' ? 'edit' : 'view';

    const dialogRef = this.dialog.open<PlanDetailDialog, PlanDetailDialogData, PlanDetailDialogResult>(
      PlanDetailDialog,
      {
        data: { plan, mode },
        width: '800px',
        maxWidth: '90vw',
        disableClose: true
      }
    );

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (result.action !== 'close') {
          this.fetchPlans();
        }
      }
    });
  }

  onRetryPlan(planGenerationId: string): void {
    const plan = this.getPlanByGenerationId(planGenerationId);

    if (!plan) {
      this.notificationService.notifyWarning('Plan not found, refresh the page and try again.');
      return;
    }

    const dialogRef = this.dialog.open<ConfirmationDialog, ConfirmationDialogData, boolean>(
      ConfirmationDialog,
      {
        data: {
          title: 'Retry Plan Generation',
          message: 'Are you sure you want to retry generating this plan? This will create a new generation attempt.',
          confirmText: 'Retry',
          cancelText: 'Cancel'
        },
        width: '400px'
      }
    );

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.retryPlan(planGenerationId);
      }
    });
  }

  onDeletePlan(planId: string): void {
    const dialogRef = this.dialog.open<ConfirmationDialog, ConfirmationDialogData, boolean>(
      ConfirmationDialog,
      {
        data: {
          title: 'Delete Plan',
          message: 'Are you sure you want to delete this plan? This action cannot be undone.',
          confirmText: 'Delete',
          cancelText: 'Cancel'
        },
        width: '400px'
      }
    );

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.deletePlan(planId);
      }
    });
  }

  retryPlan(planGenerationId: string): void {
    this.apiService.retryPlanGeneration(planGenerationId)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => {
          this.notificationService.notifySuccess('Plan generation retry started!');
          this.fetchPlans();
        },
        error: (error) => this.notificationService.notifyError('Failed to retry plan generation.'),
      });
  }

  deletePlan(planId: string): void {
    this.apiService.deletePlan(planId)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => {
          this.notificationService.notifySuccess('Plan deleted successfully!');
          this.fetchPlans();
        },
        error: (error) => this.notificationService.notifyError('Failed to delete plan.'),
      });
  }

  private getPlanById = (planId: string): Plan | undefined => this.plans()?.find(p => p.id === planId);

  private getPlanByGenerationId = (planGenerationId: string): Plan | undefined => this.plans()?.find(p => p.planGenerationId === planGenerationId);
}
