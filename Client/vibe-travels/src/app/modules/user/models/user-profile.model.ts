export interface UserProfile {
    travelStyle: string | null;
    accommodationPreference: string | null;
    climatePreference: string | null;
    lastUpdatedAt: Date | null;
}

export interface UserProfileFormData {
    travelStyle: string;
    accommodationPreference: string;
    climatePreference: string;
}