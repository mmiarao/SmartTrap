import { AfterViewInit, Directive, Input, OnDestroy} from '@angular/core';
import { MatCheckbox } from '@angular/material';
import { Subject } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';

@Directive({
  selector: 'mat-checkbox[mat-readonly]'
})
export class MatCheckBoxReadOnlyDirective implements OnDestroy, AfterViewInit {

  private readonly _onDestroy$ = new Subject<void>();
  private _readonly = false;
  private _checked = false;
  private _indeterminate = false;

  get readonly() {
    return this._readonly;
  }

  @Input('mat-readonly')
  set readonly(value: boolean) {
    this._readonly = value;
    this._apply();
  }

  constructor(private readonly _checkbox: MatCheckbox) {
  }

  ngOnDestroy() {
    this._onDestroy$.next();
  }

  ngAfterViewInit() {
    // 値が変更されたら元に戻す
    this._checkbox.change
      .pipe(
        takeUntil(this._onDestroy$),
        filter(() => this.readonly),
      )
      .subscribe(() => this._checkbox.checked = this._checked);
    this._checkbox.indeterminateChange
      .pipe(
        takeUntil(this._onDestroy$),
        filter(() => this.readonly),
      )
      .subscribe(() => this._checkbox.indeterminate = this._indeterminate);

    this._apply();
  }

  private _apply() {
    // Rippleを消すことでクリックできた感を無くす
    this._checkbox.disableRipple = this.readonly;

    this._checked = this._checkbox.checked;
    this._indeterminate = this._checkbox.indeterminate;
  }

}