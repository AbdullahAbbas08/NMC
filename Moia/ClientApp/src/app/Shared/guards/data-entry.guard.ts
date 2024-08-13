import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../Services/auth.service';
import { BrowserStorageService } from '../Services/browser-storage.service';

@Injectable({
  providedIn: 'root'
})
export class DataEntryGuard implements CanActivate {
  constructor(private authSvr: AuthService, private router: Router, private browser: BrowserStorageService) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    let userPermissions = this.authSvr.getUserPermissions();
    if (this.authSvr.isAuthenticated$.getValue() && this.authSvr.userPermissions$.getValue().length > 0 && userPermissions.includes('DataEntry')) {
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }

}
