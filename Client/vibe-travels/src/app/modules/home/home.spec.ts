import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { Home } from './home';

describe('Home', () => {
  let component: Home;
  let fixture: ComponentFixture<Home>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Home],
      providers: [provideRouter([])]
    }).compileComponents();

    fixture = TestBed.createComponent(Home);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have features defined', () => {
    expect(component.features).toBeDefined();
    expect(component.features.length).toBeGreaterThan(0);
  });

  it('should have three features', () => {
    expect(component.features.length).toBe(3);
  });

  it('should have correct feature structure', () => {
    component.features.forEach(feature => {
      expect(feature.icon).toBeDefined();
      expect(feature.title).toBeDefined();
      expect(feature.description).toBeDefined();
    });
  });
});
