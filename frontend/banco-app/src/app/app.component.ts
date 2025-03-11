import { Component } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterOutlet } from '@angular/router';

import { CommonModule } from '@angular/common';
import { filter } from 'rxjs';
import { FooterComponent } from "./core/footer/footer.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, FooterComponent], // Importa RouterOutlet y los componentes
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  showNavbar = true;
  showFooter = true;

  constructor(private router: Router, private activatedRoute: ActivatedRoute) {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        const routeData = this.getRouteData();
        this.showNavbar = routeData['showNavbar'] ?? true;
        this.showFooter = routeData['showFooter'] ?? true;
      });
  }

  private getRouteData() {
    let route = this.activatedRoute.firstChild;
    while (route?.firstChild) {
      route = route.firstChild;
    }
    return route?.snapshot.data || {};
  }
}
