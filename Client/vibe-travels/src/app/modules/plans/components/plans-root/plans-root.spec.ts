import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { PlansRoot } from './plans-root';
import { of } from 'rxjs';
import { PlansApiService } from '../../services/plans-api.service';

describe('PlansRoot', () => {
  let component: PlansRoot;
  let fixture: ComponentFixture<PlansRoot>;
  let apiService: PlansApiService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlansRoot],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();

    apiService = TestBed.inject(PlansApiService);
    
    // Mock API call before creating component
    jest.spyOn(apiService, 'getPlans').mockReturnValue(of([]));
    
    fixture = TestBed.createComponent(PlansRoot);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    component.ngOnDestroy();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize plans signal', () => {
    expect(component.plans).toBeDefined();
  });

  it('should initialize loadingData signal', () => {
    expect(component.loadingData).toBeDefined();
  });

  it('should not be loading data initially', () => {
    expect(component.loadingData()).toBe(false);
  });

  it('should fetch plans on initialization', () => {
    expect(apiService.getPlans).toHaveBeenCalled();
  });

  it('should clean up on destroy', () => {
    const spy = jest.spyOn(component as any, 'stopAutoRefresh');
    component.ngOnDestroy();
    expect(spy).toHaveBeenCalled();
  });

  it('should have onViewPlan method', () => {
    expect(component.onViewPlan).toBeDefined();
    expect(typeof component.onViewPlan).toBe('function');
  });

  it('should have onRetryPlan method', () => {
    expect(component.onRetryPlan).toBeDefined();
    expect(typeof component.onRetryPlan).toBe('function');
  });

  it('should have fetchPlans method', () => {
    expect(component.fetchPlans).toBeDefined();
    expect(typeof component.fetchPlans).toBe('function');
  });
});
