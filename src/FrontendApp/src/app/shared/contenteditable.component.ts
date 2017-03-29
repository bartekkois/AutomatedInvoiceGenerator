import { Component, NgModule, forwardRef, ViewChild, ElementRef, Input } from '@angular/core';
import { FormsModule, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
    selector: 'contenteditable-input',
    template: `
                <div #name [contentEditable]="isEditable" [class.isEditable]="isEditable" (keyup)="onChange()" [innerHtml]="_value"></div>
              `,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => ContenteditableComponent),
            multi: true
        }
    ]
})
export class ContenteditableComponent implements ControlValueAccessor {
    @ViewChild('name') contenteditableDiv: ElementRef;
    @Input() isEditable: boolean = false;
    private _value: string = '';

    private _onChange = (_) => { };
    private _onTouched = () => { };

    onChange() {
        this._value = this.contenteditableDiv.nativeElement.innerText;

        if (this._onChange) {
            this._onChange(this._value);
        }
    }

    // Set accessor including the onChange callback
    set value(value: any) {
        if (value !== this._value) {
            this._value = value;
            this._onChange(value);
        }
    }

    // Called if a value comes in via ngModule
    writeValue(value: any) {
        this._value = value;
    }

    registerOnChange(fn: (_: any) => void): void { this._onChange = fn; }
    registerOnTouched(fn: () => void): void { this._onTouched = fn; }
}