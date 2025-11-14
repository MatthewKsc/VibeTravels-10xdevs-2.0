import { Component, input, output } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { DatePipe } from '@angular/common';
import { Plan } from '../../models/plan.model';

@Component({
  selector: 'app-plan-card',
  imports: [MatCardModule, MatIconModule, MatChipsModule, MatButtonModule, MatProgressSpinnerModule, DatePipe],
  templateUrl: './plan-card.html',
  styleUrl: './plan-card.scss'
})
export class PlanCard {
  plan = input.required<Plan>();
  
  viewPlanClick = output<string>();
  retryPlanClick = output<string>();
  deletePlanClick = output<string>();
  acceptPlanClick = output<string>();
  rejectPlanClick = output<string>();

  isGenerating(): boolean {
    const status = this.plan().generationStatus;
    return status !== 'failed' && (status === 'queued' || status === 'running' || this.plan().decisionStatus === 'notgenerated');
  }

  isFailed = (): boolean => this.plan().generationStatus === 'failed';

  isGenerated = (): boolean => this.plan().generationStatus === 'succeeded' && this.plan().decisionStatus === 'generated';

  isAccepted = (): boolean => this.plan().decisionStatus === 'accepted';

  isRejected = (): boolean => this.plan().decisionStatus === 'rejected';

  onViewPlan = (): void => this.viewPlanClick.emit(this.plan().id);

  onRetry = (): void => this.retryPlanClick.emit(this.plan().planGenerationId);

  onDelete = (): void => this.deletePlanClick.emit(this.plan().id);

  onAccept = (): void => this.acceptPlanClick.emit(this.plan().id);

  onReject = (): void => this.rejectPlanClick.emit(this.plan().id);
}
