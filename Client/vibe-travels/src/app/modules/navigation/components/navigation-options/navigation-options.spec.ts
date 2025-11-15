import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NavigationOptions } from './navigation-options';

describe('NavigationOptions', () => {
  let component: NavigationOptions;
  let fixture: ComponentFixture<NavigationOptions>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavigationOptions],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(NavigationOptions);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('isLoggedIn', false);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have authenticated options', () => {
    expect(component.authenticatedOptions()).toBeDefined();
    expect(component.authenticatedOptions().length).toBeGreaterThan(0);
  });

  it('should have unauthenticated options', () => {
    expect(component.unauthenticatedOptions()).toBeDefined();
    expect(component.unauthenticatedOptions().length).toBeGreaterThan(0);
  });

  it('should show unauthenticated options when not logged in', () => {
    fixture.componentRef.setInput('isLoggedIn', false);
    fixture.detectChanges();
    expect(component.options()).toEqual(component.unauthenticatedOptions());
  });

  it('should show authenticated options when logged in', () => {
    fixture.componentRef.setInput('isLoggedIn', true);
    fixture.detectChanges();
    expect(component.options()).toEqual(component.authenticatedOptions());
  });

  it('should have signOut method', () => {
    expect(component.signOut).toBeDefined();
    expect(typeof component.signOut).toBe('function');
  });
});
