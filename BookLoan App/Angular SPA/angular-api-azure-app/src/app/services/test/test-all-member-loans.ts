import { AllMemberLoansReport } from 'src/app/models/AllMemberLoansReport';

export function getAllMemberLoans(): AllMemberLoansReport[] {
    return [
    { title: 'Lord of the Rings', borrower: 'andyh', status: 'ON LOAN', dateLoaned: new Date(), dateDue: new Date() },
    { title: 'The Hobbit', borrower: 'billb', status: 'ON LOAN', dateLoaned: new Date(), dateDue: new Date() },
    { title: 'Of Mice and Men', borrower: 'sallyd', status: 'ON LOAN', dateLoaned: new Date(), dateDue: new Date() }
  ];
}
