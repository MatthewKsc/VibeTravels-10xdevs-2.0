import { Plan } from "./plan.model";

export interface PlanDetailDialogData {
  plan: Plan;
  mode: 'view' | 'edit';
}

export type PlanDetailDialogResult = {
  action: 'accept' | 'reject' | 'update' | 'close';
  reason?: string;
};