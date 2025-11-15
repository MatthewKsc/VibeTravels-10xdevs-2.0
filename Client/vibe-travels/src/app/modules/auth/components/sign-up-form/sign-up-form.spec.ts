import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { SignUpForm } from './sign-up-form';

describe('SignUpForm', () => {
  let component: SignUpForm;
  let fixture: ComponentFixture<SignUpForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SignUpForm],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(SignUpForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with empty fields', () => {
    expect(component.signUpForm.get('email')?.value).toBe('');
    expect(component.signUpForm.get('password')?.value).toBe('');
    expect(component.signUpForm.get('confirmPassword')?.value).toBe('');
  });

  it('should have invalid form when fields are empty', () => {
    expect(component.signUpForm.valid).toBe(false);
  });

  it('should have passwords hidden by default', () => {
    expect(component.hidePassword()).toBe(true);
    expect(component.hideConfirmPassword()).toBe(true);
  });

  it('should not be processing request initially', () => {
    expect(component.processingRequest()).toBe(false);
  });

  it('should show passwords match by default', () => {
    expect(component.passwordsMatch()).toBe(true);
  });

  it('should toggle password visibility', () => {
    const initialValue = component.hidePassword();
    component.hidePassword.set(!initialValue);
    expect(component.hidePassword()).toBe(!initialValue);
  });
});
