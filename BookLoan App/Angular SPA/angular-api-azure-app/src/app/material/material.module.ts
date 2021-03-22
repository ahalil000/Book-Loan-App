import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatInputModule, MatFormFieldModule, MatTabsModule, MatSidenavModule, 
         MatToolbarModule, MatIconModule, MatButtonModule, MatListModule, 
         MatMenuModule, MatSelectModule, MatButtonToggleModule,
         MatDatepickerModule, MatNativeDateModule, MatCardModule,
         MatDialogModule, MatPaginatorModule, MatSortModule, MatGridListModule, 
         MatTableModule, MatProgressSpinnerModule, MatSnackBarModule, 
         MatBadgeModule, MAT_DATE_LOCALE, MatCheckboxModule, MatChipsModule } from '@angular/material'; 

@NgModule({
  imports: [
    MatMenuModule,
    MatListModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatButtonToggleModule,
    MatDatepickerModule,
    MatNativeDateModule,
    CommonModule,
    MatTabsModule,
    MatSidenavModule,
    MatCardModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatPaginatorModule,
    MatSortModule,
    MatTableModule,
    MatSnackBarModule,
    MatBadgeModule,
    MatGridListModule,
    MatCheckboxModule,
    MatChipsModule
  ],
  exports: [
    MatMenuModule,
    MatListModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatButtonToggleModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTabsModule,
    MatSidenavModule,
    MatCardModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatPaginatorModule,
    MatSortModule,
    MatTableModule,
    MatSnackBarModule,
    MatBadgeModule,
    MatGridListModule,
    MatCheckboxModule,
    MatChipsModule
  ],
  providers: [
    {provide: MAT_DATE_LOCALE, useValue: 'en-GB'},
  ],
  declarations: []
})
export class MaterialModule { }
