export type PlanGenerationStatus = 'queued' | 'running' | 'succeeded' | 'failed';
export type PlanDecisionStatus = 'notgenerated' | 'generated' | 'accepted' | 'rejected';

export interface Plan {
    id: string;
    planGenerationId: string;
    travelers: number;
    travelDays: number;
    startDate: Date;
    structureType: string;
    generationStatus: PlanGenerationStatus;
    decisionStatus: PlanDecisionStatus;
    contentMd: string | null;
    errorMessage: string | null;
    lastUpdatedAt: Date;
}

export interface PlanFormData {
    noteId: string;
    travelDays: number;
    travelers: number;
    startDate: Date;
}

export interface PlanDecision {
    decisionReason: string;
}

export interface PlanContentUpdate {
    contentMd: string;
}