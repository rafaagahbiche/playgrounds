import { Photo } from './Photo';

export interface Member {
    loginname: string;
    emailadress: string;
    password: string;
    photos?: Photo[];
}
