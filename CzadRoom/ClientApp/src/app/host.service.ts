import { inject, Injectable } from '@angular/core'

@Injectable({ providedIn: 'root' })
export class HostConfig {
    baseURL: string

    constructor() {
        this.baseURL = document.getElementsByTagName('base')[0].href
    }
}