import { Book } from './Book';

export class TopBooksLoanedReportViewModel
{
    public title: string;
    public ranking: number;
    public count: number
    public averageRating: string;
    public bookDetails: Book;
}