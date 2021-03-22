import { UserAccount } from "src/app/models/UserAccount";

export function getUsers(): UserAccount[] {
  return [
    { id: '1', userName: 'andyh', email: 'andyh@aaa.com', password: 'p@ss123', firstName: 'Andy', lastName: 'Hall', dob: new Date() },
    { id: '2', userName: 'billb', email: 'billb@aaa.com', password: 'p@ss123', firstName: 'Bill', lastName: 'Bloggs', dob: new Date() },
    { id: '3', userName: 'sallyd', email: 'sallyd@aaa.com', password: 'p@ss123', firstName: 'Sally', lastName: 'Dart', dob: new Date() },
  ];
}
