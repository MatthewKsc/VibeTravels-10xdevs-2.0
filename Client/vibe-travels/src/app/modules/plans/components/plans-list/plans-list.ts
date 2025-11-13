import { Component, input, output } from '@angular/core';
import { Plan } from '../../models/plan.model';
import { PlanCard } from '../plan-card/plan-card';

@Component({
  selector: 'app-plans-list',
  imports: [PlanCard],
  templateUrl: './plans-list.html',
  styleUrl: './plans-list.scss'
})
export class PlansList {
  plans = input.required<Plan[]>();
  
  viewPlanClick = output<string>();
  retryPlanClick = output<string>();
  deletePlanClick = output<string>();
  acceptPlanClick = output<string>();
  rejectPlanClick = output<string>();

  onViewPlan = (planId: string): void => this.viewPlanClick.emit(planId);

  onRetryPlan = (planId: string): void => this.retryPlanClick.emit(planId);

  onDeletePlan = (planId: string): void => this.deletePlanClick.emit(planId);

  onAcceptPlan = (planId: string): void => this.acceptPlanClick.emit(planId);

  onRejectPlan = (planId: string): void => this.rejectPlanClick.emit(planId);
}
