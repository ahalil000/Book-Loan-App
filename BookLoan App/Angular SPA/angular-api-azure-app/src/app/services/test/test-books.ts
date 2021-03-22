import { Book } from '../../models/Book';

/** return fresh array of test heroes */
export function getBooks(): Book[] {
  return [
    { id: 1, title: 'Lord of the Rings', author: '', genre: '', isbn: '', yearPublished: 2020, edition: '', location: '', dateCreated: new Date(), dateUpdated: new Date() },
    { id: 2, title: 'The Hobbit', author: '', genre: '', isbn: '', yearPublished: 2020, edition: '', location: '', dateCreated: new Date(), dateUpdated: new Date() },
    { id: 3, title: 'Of Mice and Men', author: '', genre: '', isbn: '', yearPublished: 2020, edition: '', location: '', dateCreated: new Date(), dateUpdated: new Date() }
  ];
}
