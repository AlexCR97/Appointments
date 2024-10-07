import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-portal',
  standalone: true,
  imports: [RouterOutlet, RouterLink],
  templateUrl: './portal.page.html',
  styleUrl: './portal.page.scss',
})
export class PortalPage {}
