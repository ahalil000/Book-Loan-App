import { TopBooksLoanedReportViewModel } from 'src/app/models/TopBooksLoanedReport';
import { getBooks } from './test-books';

export function getTopBooksLoaned(): TopBooksLoanedReportViewModel[] {
    var book1 = getBooks().find(b => b.id === 1);
    var book2 = getBooks().find(b => b.id === 2);
    var book3 = getBooks().find(b => b.id === 3);
    return [
      { title: book1.title, ranking: 1, count: 3, averageRating: '1', bookDetails: book1 },
      { title: book2.title, ranking: 2, count: 2, averageRating: '2', bookDetails: book2 },
      { title: book3.title, ranking: 3, count: 1, averageRating: '3', bookDetails: book3 }
    ];
}
