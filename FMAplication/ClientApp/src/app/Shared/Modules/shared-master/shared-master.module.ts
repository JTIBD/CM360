import { ImageViewModalComponent } from './../image-view-modal/image-view-modal.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageTitleComponent } from 'src/app/Layout/LayoutComponent/Components/page-title/page-title.component';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { AlertModule } from '../alert/alert.module';
import { PTableModule } from '../p-table/p-table.module';
import { ImageUploaderComponent } from 'src/app/Shared/Modules/image-uploader/image-upload.component';
import { PaginatorModule } from 'primeng/paginator';
import { NumberDirective } from 'src/app/Shared/Directive/numbers-only.directive';

@NgModule({
    declarations: [
        PageTitleComponent,
        ImageUploaderComponent, 
        ImageViewModalComponent,
        NumberDirective
    ],
    imports: [
        CommonModule,
        AngularFontAwesomeModule,
        AlertModule,
        PTableModule,
        //PaginatorModule
    ],
    exports: [PageTitleComponent,
        ImageUploaderComponent,
        ImageViewModalComponent,
        AlertModule,
        PTableModule,
        NumberDirective
        //PaginatorModule
    ], 
    entryComponents: [
        ImageViewModalComponent,
    ]
})
export class SharedMasterModule { }
