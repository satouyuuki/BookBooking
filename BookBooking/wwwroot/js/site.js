const uri = 'api/Books';
let books = [];

function getBooks() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addBook() {
    const addTitleTextbox = document.getElementById('title');
    const addDescriptionTextarea = document.getElementById('description');
    const addImageUrlTextbox = document.getElementById('imageUrl');

    console.log('addDescriptionTextarea = ', addDescriptionTextarea);
    const book = {
        title: addTitleTextbox.value.trim(),
        description: addDescriptionTextarea.value,
        imageUrl: addImageUrlTextbox.value.trim(),
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(book)
    })
        .then(response => response.json())
        .then(() => {
            getBooks();
            addTitleTextbox.value = '';
            addDescriptionTextarea.value = '';
            addImageUrlTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteBook(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getBooks())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const book = books.find(item => item.id === id);

    document.getElementById('editId').value = book.id;
    document.getElementById('editTitle').value = book.title;
    document.getElementById('editDescription').value = book.description;
    document.getElementById('editImageUrl').value = book.imageUrl;
    document.getElementById('editForm').style.display = 'block';
}

function updateBook() {
    const bookId = document.getElementById('editId').value;
    const book = {
        id: parseInt(bookId, 10),
        title: document.getElementById('editTitle').value.trim(),
        description: document.getElementById('editDescription').value,
        imageUrl: document.getElementById('editImageUrl').value.trim()
    };

    fetch(`${uri}/${bookId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(book)
    })
        .then(() => getBooks())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'book' : 'books';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('books');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(book => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${book.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteBook(${book.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let titleNode = document.createTextNode(book.title);
        td1.appendChild(titleNode);

        let td2 = tr.insertCell(1);
        let descriptionNode = document.createTextNode(book.description);
        td2.appendChild(descriptionNode);

        let td3 = tr.insertCell(2);
        let imageUrlNode = document.createTextNode(book.imageUrl);
        td3.appendChild(imageUrlNode);

        let td4 = tr.insertCell(3);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);
    });

    books = data;
}
