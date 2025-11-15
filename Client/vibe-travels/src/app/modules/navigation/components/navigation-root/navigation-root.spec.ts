import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NavigationRoot } from './navigation-root';

describe('NavigationRoot', () => {
  let component: NavigationRoot;
  let fixture: ComponentFixture<NavigationRoot>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavigationRoot],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(NavigationRoot);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have app name defined', () => {
    expect(component.appName).toBe('VibeTravels');
  });

  it('should have isLoggedIn signal', () => {
    expect(component.isLoggedIn).toBeDefined();
  });

  it('should initialize isLoggedIn to false', () => {
    expect(component.isLoggedIn()).toBe(false);
  });
});
