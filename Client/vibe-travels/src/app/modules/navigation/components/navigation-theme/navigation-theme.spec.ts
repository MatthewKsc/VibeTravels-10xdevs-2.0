import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NavigationTheme } from './navigation-theme';

describe('NavigationTheme', () => {
  let component: NavigationTheme;
  let fixture: ComponentFixture<NavigationTheme>;

  beforeEach(async () => {
    // Clear localStorage before each test
    localStorage.clear();

    await TestBed.configureTestingModule({
      imports: [NavigationTheme]
    }).compileComponents();

    fixture = TestBed.createComponent(NavigationTheme);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    localStorage.clear();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with light theme by default', () => {
    expect(component.currentTheme()).toBe('light');
  });

  it('should toggle theme from light to dark', () => {
    component.currentTheme.set('light');
    component.toggleTheme();
    expect(component.currentTheme()).toBe('dark');
  });

  it('should toggle theme from dark to light', () => {
    component.currentTheme.set('dark');
    component.toggleTheme();
    expect(component.currentTheme()).toBe('light');
  });

  it('should have toggleTheme method', () => {
    expect(component.toggleTheme).toBeDefined();
    expect(typeof component.toggleTheme).toBe('function');
  });
});
