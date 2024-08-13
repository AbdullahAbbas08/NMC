import { Directive, Input, TemplateRef, ViewContainerRef, OnInit } from '@angular/core';
import { BrowserStorageService } from '../Services/browser-storage.service';
import { AuthService } from '../Services/auth.service';

@Directive({
  selector: '[appHasPermission]'
})
export class HasPermissionDirective implements OnInit {
  @Input('appHasPermission') permissions: string[];
  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private browser: BrowserStorageService,
    private auth: AuthService
  ) { }

  ngOnInit() {
    // Check if the user has any of the specified permissions
    const hasPermission = this.permissions.some(permission =>
      this.userPermissions.includes(permission)
    );
    // Show or hide the element based on user's permissions
    if (hasPermission) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }

  private get userPermissions(): string[] {
    const permissionsJson = this.auth.userPermissions$.getValue();
    const permissions: string[] = permissionsJson ? permissionsJson : [];
    return permissions;
  }

}
