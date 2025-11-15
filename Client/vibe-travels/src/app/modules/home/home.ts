import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from "@angular/router";
import { IHomeFeature } from './home.model';

@Component({
  selector: 'app-home',
  imports: [MatCardModule, MatButtonModule, MatIconModule, RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home {
  readonly features: IHomeFeature[] = [
    {
      icon: 'description',
      title: 'Capture Trip Ideas',
      description: 'Create and organize notes about destinations you want to visit. Store all your travel inspiration in one place.',
    },
    {
      icon: 'auto_awesome',
      title: 'AI-Powered Planning',
      description: 'Transform your notes into detailed itineraries. Get day-by-day plans or activity lists tailored to your preferences.',
    },
    {
      icon: 'tune',
      title: 'Personalized Experience',
      description: 'Set your travel style, accommodation preferences, and climate choices to get plans that match your needs.',
    },
  ];
}
