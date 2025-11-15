import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { UserProfile } from './user-profile';
import { of } from 'rxjs';
import { UserProfileApiService } from '../../services/user-profile-api.service';
import { TravelStyle, Accommodation, ClimateRegion } from '../../models/user-profile.enum';

describe('UserProfile', () => {
  let component: UserProfile;
  let fixture: ComponentFixture<UserProfile>;
  let apiService: UserProfileApiService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserProfile],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();

    apiService = TestBed.inject(UserProfileApiService);
    
    // Mock API call before creating component
    jest.spyOn(apiService, 'getUserProfile').mockReturnValue(of(null));
    
    fixture = TestBed.createComponent(UserProfile);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with empty fields', () => {
    expect(component.profileForm.get('travelStyle')?.value).toBe('');
    expect(component.profileForm.get('accommodationPreference')?.value).toBe('');
    expect(component.profileForm.get('climatePreference')?.value).toBe('');
  });

  it('should have travel styles defined', () => {
    expect(component.travelStyles).toBeDefined();
    expect(component.travelStyles.length).toBeGreaterThan(0);
  });

  it('should have accommodations defined', () => {
    expect(component.accommodations).toBeDefined();
    expect(component.accommodations.length).toBeGreaterThan(0);
  });

  it('should have climate regions defined', () => {
    expect(component.climateRegions).toBeDefined();
    expect(component.climateRegions.length).toBeGreaterThan(0);
  });

  it('should initialize loadingData signal', () => {
    expect(component.loadingData).toBeDefined();
  });

  it('should initialize processingRequest signal', () => {
    expect(component.processingRequest).toBeDefined();
  });

  it('should not be loading data initially', () => {
    expect(component.loadingData()).toBe(false);
  });

  it('should not be processing request initially', () => {
    expect(component.processingRequest()).toBe(false);
  });

  it('should have invalid form when fields are empty', () => {
    expect(component.isFormValid()).toBe(false);
  });

  it('should fetch profile on initialization', () => {
    expect(apiService.getUserProfile).toHaveBeenCalled();
  });
});
