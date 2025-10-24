import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'contact',
  standalone: true
})
export class ContactPipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    return null;
  }

}
