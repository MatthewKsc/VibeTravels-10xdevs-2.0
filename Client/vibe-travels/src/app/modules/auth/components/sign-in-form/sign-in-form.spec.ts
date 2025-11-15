import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { SignInForm } from './sign-in-form';

describe('SignInForm', () => {
  let component: SignInForm;
  let fixture: ComponentFixture<SignInForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SignInForm],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(SignInForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with empty fields', () => {
    expect(component.signInForm.get('email')?.value).toBe('');
    expect(component.signInForm.get('password')?.value).toBe('');
  });

  it('should have invalid form when fields are empty', () => {
    expect(component.signInForm.valid).toBe(false);
  });

  it('should have password hidden by default', () => {
    expect(component.hidePassword()).toBe(true);
  });

  it('should not be processing request initially', () => {
    expect(component.processingRequest()).toBe(false);
  });

  it('should toggle password visibility', () => {
    const initialValue = component.hidePassword();
    component.hidePassword.set(!initialValue);
    expect(component.hidePassword()).toBe(!initialValue);
  });
});
