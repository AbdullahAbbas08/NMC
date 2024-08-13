import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { BrowserStorageService } from './browser-storage.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SwaggerClient, UserDto } from './Swagger/SwaggerClient.service';
import { OverlaySpinnerService } from '../modules/overlay-spinner/overlay-spinner.service';
import { HttpErrorResponse } from '@angular/common/http';
import { UserRoleDto } from '../models/user-roles-dto';
import { EncryptDecryptService } from './encrypt-decrypt.service';
import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  isAuthenticated$: BehaviorSubject<any> = new BehaviorSubject(false);
  currentUserRole$: BehaviorSubject<UserRoleDto | null> = new BehaviorSubject<UserRoleDto | null>(null);
  User$: BehaviorSubject<any | null> = new BehaviorSubject<any | null>(null);
  userObs$ = this.User$.asObservable();
  tokenExpirationTime: ReturnType<typeof setTimeout> | null = null;

  userPermissions$: BehaviorSubject<string[]> =
    new BehaviorSubject<string[]>([]);
  private _userFullName: string | null = null;
  get userFullName(): string | null {
    return this._userFullName;
  }

  constructor(
    private browserStorage: BrowserStorageService,
    private route: ActivatedRoute,
    private router: Router,
    private swagger: SwaggerClient,
    private overlaySpinnerSvr: OverlaySpinnerService,
    private cryptoSvr: EncryptDecryptService
  ) { }

  private async isTokenValid(): Promise<boolean> {
    if (this.browserStorage.getToken()) {
      const decodedToken: any = jwt_decode(
        this.browserStorage.getToken() as string
      );

      this.User$.next(JSON.parse(this.cryptoSvr.decryptUsingAES256(
        this.cryptoSvr.decryptUsingAES256(
          this.cryptoSvr.unshiftString(decodedToken.KkxR12WKVQOVCuvoJ5vZ3yOrGJZa8GaUcZbgokT4uPM, 4)))));
      // user name
      this._userFullName = this.cryptoSvr.decryptUsingAES256(
        this.cryptoSvr.decryptUsingAES256(
          this.cryptoSvr.unshiftString(decodedToken.NNPO12WKVQOVCuvoJ5vZ3yOrGJkia8GaUcZbgokT4uPM, 4)));

      if (Date.now() < decodedToken.exp * 1000) {
        this.isAuthenticated$.next(true);
        await this.setPermissions(decodedToken.NNPO1QQQ2FFFWKVQOVCuvRJ5vZu3yOrTGJkia8GaUOcZbgokTN4uPM);
        const expireDuration =
          decodedToken.exp * 1000 - Date.now().valueOf() - 30000;
        this.autoLogout(expireDuration);
        return true;
      } else {
        this.logout();
        return false;
      }
    } else {
      return false;
    }
  }

  private autoLogout(expireDuration: number) {
    if (this.tokenExpirationTime) {
      clearTimeout(this.tokenExpirationTime);
    }
    this.tokenExpirationTime = null;
    this.tokenExpirationTime = setTimeout(() => {
      this.updateToken();
    }, expireDuration);
  }

  setPermissions(UserRole: any) {
    try {
      const decryptedString: string = this.cryptoSvr.decryptUsingAES256(this.cryptoSvr.decryptUsingAES256(this.cryptoSvr.unshiftString(UserRole, 9)));
      const permissions: any = JSON.parse(decryptedString);
      this.userPermissions$.next(permissions);
    } catch (error) {

    }
  }


  getUserPermissions(): string[] {
    const permissionsJson = this.userPermissions$.getValue();
    const permissions: string[] = permissionsJson ? permissionsJson : [];
    return permissions;
  }

  private updateToken() {
    this.overlaySpinnerSvr.show();
    this.swagger
      .apiUserRefreshTokenPost({
        refreshToken: this.browserStorage.getRefreshToken(),
      })
      .subscribe(
        (res) => {
          if (res.access_token && res.refresh_token) {
            this.browserStorage.setToken(res.access_token, true);
            this.browserStorage.setRefreshToken(res.refresh_token, true);
            this.initAuth();
          }
          this.overlaySpinnerSvr.hide();
        },
        (err) => {
          this.overlaySpinnerSvr.hide();
        }
      );
  }


  async initAuth() {
    await this.isTokenValid();
    // this.isUserRole();
  }

  async login(token: string, refreshToken: string, rememberMe: boolean) {

    this.browserStorage.setToken(token, rememberMe);
    this.browserStorage.setRefreshToken(refreshToken, rememberMe);
    await this.isTokenValid();
    let userPermissions = this.getUserPermissions();
    if (userPermissions.includes('DataEntry')) {
      const returnUrl = '/order' || this.route.snapshot.queryParams['returnUrl'];
      this.router.navigate([returnUrl]);
    }
    else if (userPermissions.includes('CommitteeManager')) {
      const returnUrl = '/order' || this.route.snapshot.queryParams['returnUrl'];
      this.router.navigate([returnUrl]);
    }
    else if (userPermissions.includes('DepartmentManager')) {
      const returnUrl = '/order' || this.route.snapshot.queryParams['returnUrl'];
      this.router.navigate([returnUrl]);
    }
    else if (userPermissions.includes('BranchManager')) {
      const returnUrl = '/order' || this.route.snapshot.queryParams['returnUrl'];
      this.router.navigate([returnUrl]);
    }
    else if (userPermissions.includes('SuperAdmin')) {
      const returnUrl = '/user' || this.route.snapshot.queryParams['returnUrl'];
      this.router.navigate([returnUrl]);
    }
    else if (userPermissions.includes('BranchDataEntry')) {
      const returnUrl = '/user' || this.route.snapshot.queryParams['returnUrl'];
      this.router.navigate([returnUrl]);
    }
  }

  logout() {
    this.overlaySpinnerSvr.show();
    this.swagger
      .apiUserLogoutGet(this.browserStorage.getRefreshToken())
      .subscribe(
        (res) => {
          this.isAuthenticated$.next(false);
          this.currentUserRole$.next(null);
          this.browserStorage.removeToken();
          localStorage.clear()
          this.router.navigate(['/login']);
          if (this.tokenExpirationTime) {
            clearTimeout(this.tokenExpirationTime);
          }
          this.tokenExpirationTime = null;
          this.overlaySpinnerSvr.hide();
          window.location.reload()
        },
        (err: HttpErrorResponse) => {
          this.overlaySpinnerSvr.hide();
        }
      );
  }

}
