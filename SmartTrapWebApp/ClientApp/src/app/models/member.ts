export class Member {
    id:string;
    name:string;
    email:string;
    phone:string;
    line:string;
    useEmail:boolean;
    useLine:boolean;
}
export class MembersResponse {
    members: Member[];
    nextKey: string;
}
