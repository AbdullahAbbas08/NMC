import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { MegaMenuItem } from 'primeng/api';
import { AuthService } from 'src/app/Shared/Services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  sidebarVisible: boolean = false;
  items: MegaMenuItem[] = [];
  userPermissions: string[] = []
  constructor(private router: Router,
    private auth: AuthService,
    private authService: AuthService,
    private translate: TranslateService) { }

  ngOnInit(): void {
    this.userPermissions = this.authService.getUserPermissions();

    this.items.push(  // profile
      {
        label: this.translate.instant('welcome'), icon: 'pi pi-fw pi-users',
        items: [
          [
            {
              label: this.authService.User$.value['Name'],
              items: [
                { label: this.translate.instant('personaldata'), routerLink: '/Profile', icon: 'pi pi-user' },
                { label: this.translate.instant('logout'), icon: 'pi pi-power-off', command: (event: Event) => { this.logout() } }
              ]
            }

          ]
        ]
      },
    )

    if (this.userPermissions.includes('SuperAdmin')) {

      // settings
      this.items.push(
        {
          label: this.translate.instant('settings'), icon: 'pi pi-fw pi-cog',
          items: [
            [
              {
                label: this.translate.instant('addData'),
                items: [
                  { label: this.translate.instant('users'), routerLink: '/user' },
                  { label: this.translate.instant('witnesses'), routerLink: '/witness' },
                  { label: this.translate.instant('committees'), routerLink: '/committe' },
                  { label: this.translate.instant('departments'), routerLink: '/department' },
                  { label: this.translate.instant('branchs'), routerLink: '/branch' },
                  { label: this.translate.instant('islamrecognitionheader'), routerLink: '/islam-recognition' },
                  { label: this.translate.instant('presherheader'), routerLink: '/Presher' },
                  { label: this.translate.instant('resedenceplace'), routerLink: '/ResidencePlace' },
                  { label: this.translate.instant('MoveOrders'), routerLink: '/MoveOrders' },
                  { label: this.translate.instant('Reports'), routerLink: '/reports' },

                  { label: this.translate.instant('localization'), routerLink: '/localization' },
                ]
              }
            ],
          ]
        },)

      //orders
      this.items.push({
        label: this.translate.instant('ordersData'), icon: 'pi pi-align-justify',
        items: [
          [
            {
              label: this.translate.instant('orders'),
              items: [
                // { label: 'إنشاء طلب جديد', routerLink: '/muslime' },
                { label: this.translate.instant('orderpreview'), routerLink: '/order' },
                // { label: 'الطلبات قيد الإنتظار', routerLink: '/order/waiting' },
                // { label: 'طلبات للمراجعة', routerLink: '/order/review' },
                // { label: ' طباعة البطاقات', routerLink: '/order/print' },
              ]
            }
          ],
        ]
      })

      //acceptance order
      // this.items.push({
      //   label: 'طلبات الموافقة', icon: 'pi pi-align-justify',
      //   items: [
      //     [
      //       {
      //         label: 'بيانات طلبات الموافقة',
      //         items: [
      //           { label: 'عرض الطلبات', routerLink: '/order' },
      //         ]
      //       }
      //     ],
      //   ]
      // })

    }

    //DataEntry
    else if (this.userPermissions.includes('DataEntry')) {
      this.items.push({
        label: this.translate.instant('orders'), icon: 'pi pi-align-justify',
        items: [
          [
            {
              label: this.translate.instant('ordersData'),
              items: [
                { label: this.translate.instant('neworder'), routerLink: '/muslime' },
                { label: this.translate.instant('savedorder'), routerLink: '/order' },
                // { label: 'الطلبات قيد الإنتظار', routerLink: '/order/waiting' },
                { label: this.translate.instant('orderreview'), routerLink: '/order/review' },
                { label: this.translate.instant('allmyOrders'), routerLink: '/order/preview-orders' },
                { label: this.translate.instant('printcard'), routerLink: '/order/print' },
              ]
            }
          ],
        ]
      })
    }

    else if (this.userPermissions.includes('BranchDataEntry')) {
      this.items.push({
        label: this.translate.instant('settings'), icon: 'pi pi-align-justify',
        items: [
          [
            {
              label: this.translate.instant('addData'),
              items: [
                { label: this.translate.instant('committees'), routerLink: '/committe' },
                { label: this.translate.instant('users'), routerLink: '/user' },
                { label: this.translate.instant('witnesses'), routerLink: '/witness' },
                { label: this.translate.instant('Reports'), routerLink: '/reports' },
                // { label: this.translate.instant('departments'), routerLink: '/department' },
                // { label: this.translate.instant('branchs'), routerLink: '/branch' },
                // { label: this.translate.instant('islamrecognitionheader'), routerLink: '/islam-recognition' },
                // { label: this.translate.instant('presherheader'), routerLink: '/Presher' },
                // { label: this.translate.instant('resedenceplace'), routerLink: '/ResidencePlace' },
                // { label: this.translate.instant('localization'), routerLink: '/localization' },
              ]
            }
          ],
        ]
      })
    }

    else if (this.userPermissions.includes('CommitteeManager') ) {

      this.items.push({
        label: this.translate.instant('orderaccept'), icon: 'pi pi-align-justify',
        items: [
          [
            {
              label: this.translate.instant('orderaccept'),
              items: [
                { label: this.translate.instant('orderpreview'), routerLink: '/order' },
                // { label: this.translate.instant('orderreview'), routerLink: '/order/review' },
                { label: this.translate.instant('allmyOrders'), routerLink: '/order/preview-orders' },
                { label: this.translate.instant('MoveOrders'), routerLink: '/MoveOrders' },
                { label: this.translate.instant('Reports'), routerLink: '/reports' },
                { label: this.translate.instant('printcard'), routerLink: '/order/print' },
              ]
            }
          ],
        ]
      })
    }
    else if (this.userPermissions.includes('CommitteeManager') ||
      this.userPermissions.includes('DepartmentManager') ||
      this.userPermissions.includes('NegoiatedDepartmentManager') ||
      this.userPermissions.includes('NegoiatedBranchManager') ||
      this.userPermissions.includes('BranchManager')
    ) {

      this.items.push({
        label: this.translate.instant('orderaccept'), icon: 'pi pi-align-justify',
        items: [
          [
            {
              label: this.translate.instant('orderaccept'),
              items: [
                { label: this.translate.instant('orderpreview'), routerLink: '/order' },
                { label: this.translate.instant('allmyOrders'), routerLink: '/order/preview-orders' },
                { label: this.translate.instant('Reports'), routerLink: '/reports' },
              ]
            }
          ],
        ]
      })
    }



  }

  navigate() {
    this.router.navigateByUrl("/Setting");
  }

  logout() {
    this.auth.logout()
  }

  showDetails: boolean = false;

  toggleDetails() {
    this.showDetails = !this.showDetails;
  }

  GoToDefault() {
    if (this.userPermissions.includes('BranchDataEntry'))
      this.router.navigateByUrl("/user");
    else
      this.router.navigateByUrl("/");
  }

}
