import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Plan, PlanContentUpdate, PlanDecision, PlanFormData } from '../models/plan.model';
import { environment } from '../../../../environments/environments';

@Injectable({ providedIn: 'root' })
export class PlansApiService {
  private httpClient = inject(HttpClient);
  private readonly plansApiUrl = environment.apiUrl + '/plans';

  getPlan(planId: string): Observable<Plan> {
    return this.httpClient.get<Plan>(`${this.plansApiUrl}/${planId}`);
  }

  getPlans(): Observable<Plan[]> {
    return this.httpClient.get<Plan[]>(this.plansApiUrl);
  }

  addPlan(requestData: PlanFormData): Observable<void> {
    return this.httpClient.post<void>(this.plansApiUrl, requestData);
  }

  acceptPlan(planId: string, decision: PlanDecision): Observable<void> {
    return this.httpClient.post<void>(`${this.plansApiUrl}/${planId}/accept`, decision);
  }

  rejectPlan(planId: string, decision: PlanDecision): Observable<void> {
    return this.httpClient.post<void>(`${this.plansApiUrl}/${planId}/reject`, decision);
  }

  retryPlanGeneration(planGenerationId: string): Observable<void> {
    return this.httpClient.post<void>(`${this.plansApiUrl}/${planGenerationId}/retry`, {});
  }

  updatePlan(planId: string, requestData: PlanContentUpdate): Observable<void> {
    return this.httpClient.put<void>(`${this.plansApiUrl}/${planId}`, requestData);
  }

  deletePlan(planId: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.plansApiUrl}/${planId}`);
  }
}
