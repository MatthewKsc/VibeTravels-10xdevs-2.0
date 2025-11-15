import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { NotesRoot } from './notes-root';
import { of } from 'rxjs';
import { NotesApiService } from '../../services/notes-api.service';

describe('NotesRoot', () => {
  let component: NotesRoot;
  let fixture: ComponentFixture<NotesRoot>;
  let apiService: NotesApiService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NotesRoot],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();

    apiService = TestBed.inject(NotesApiService);
    
    // Mock API call before creating component
    jest.spyOn(apiService, 'getNotes').mockReturnValue(of([]));
    
    fixture = TestBed.createComponent(NotesRoot);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize notes signal', () => {
    expect(component.notes).toBeDefined();
  });

  it('should initialize loadingData signal', () => {
    expect(component.loadingData).toBeDefined();
  });

  it('should not be loading data initially', () => {
    expect(component.loadingData()).toBe(false);
  });

  it('should have onAddNote method', () => {
    expect(component.onAddNote).toBeDefined();
    expect(typeof component.onAddNote).toBe('function');
  });

  it('should fetch notes on initialization', () => {
    expect(apiService.getNotes).toHaveBeenCalled();
  });
});
