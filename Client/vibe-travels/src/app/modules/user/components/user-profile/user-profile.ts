import { Component, computed, DestroyRef, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { finalize } from 'rxjs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';

import { UserProfile as UserProfileModel, UserProfileFormData } from '../../models/user-profile.model';
import { TravelStyle, Accommodation, ClimateRegion } from '../../models/user-profile.enum';
import { UserProfileApiService } from '../../services/user-profile-api.service';
import { NotificationService } from '../../../../shared/services';

@Component({
  selector: 'app-user-profile',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatIconModule
  ],
  templateUrl: './user-profile.html',
  styleUrl: './user-profile.scss',
})
export class UserProfile implements OnInit {
  private apiService = inject(UserProfileApiService);
  private formBuilder = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);
  private notificationService = inject(NotificationService);
  private router = inject(Router);

  loadingData: WritableSignal<boolean> = signal<boolean>(false);
  processingRequest: WritableSignal<boolean> = signal<boolean>(false);

  travelStyles: TravelStyle[] = Object.values(TravelStyle);
  accommodations: Accommodation[] = Object.values(Accommodation);
  climateRegions: ClimateRegion[] = Object.values(ClimateRegion);

  profileForm = this.formBuilder.group({
    travelStyle: ['', Validators.required],
    accommodationPreference: ['', Validators.required],
    climatePreference: ['', Validators.required]
  });

  isFormValid = computed(() => this.profileForm.valid);

  ngOnInit(): void {
    this.fetchUserProfile();
  }

  goBack(): void {
    this.router.navigate(['/notes']);
  }

  updateProfile(): void {
    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      return;
    }

    this.processingRequest.set(true);
    const formValue = this.profileForm.value;

    const profileData: UserProfileFormData = {
      travelStyle: formValue.travelStyle!,
      accommodationPreference: formValue.accommodationPreference!,
      climatePreference: formValue.climatePreference!
    };

    this.apiService.updateUserProfile(profileData)
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => this.processingRequest.set(false))
      )
      .subscribe({
        next: () => {
          this.notificationService.notifySuccess('Profile updated successfully!');
          this.fetchUserProfile();
        },
        error: (error) => this.notificationService.notifyError('Failed to update profile. Please try again.'),
      });
  }

  private fetchUserProfile(): void {
    this.loadingData.set(true);

    this.apiService.getUserProfile()
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => this.loadingData.set(false))
      )
      .subscribe({
        next: (profile: UserProfileModel | null) => {
          if (profile) {
            this.profileForm.patchValue({
              travelStyle: profile.travelStyle ?? '',
              accommodationPreference: profile.accommodationPreference ?? '',
              climatePreference: profile.climatePreference ?? ''
            });
          }
        },
        error: (error) => this.notificationService.notifyError('Failed to load profile. Please try again.'),
      });
  }
}
