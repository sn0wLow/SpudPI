function saveMemeSoundBitmapsToIndexedDB(jsonData) {
    const memeSoundBitmaps = JSON.parse(jsonData);
    const dbName = 'MemeDatabase';
    const storeName = 'memeSoundBitmaps';
    const request = window.indexedDB.open(dbName, 1);

    request.onupgradeneeded = function (event) {
        const db = event.target.result;
        if (!db.objectStoreNames.contains(storeName)) {
            db.createObjectStore(storeName, { keyPath: 'memeId' });
        }
    };

    request.onsuccess = function (event) {
        const db = event.target.result;
        const transaction = db.transaction(storeName, 'readwrite');
        const store = transaction.objectStore(storeName);

        memeSoundBitmaps.forEach((bitmap) => {
            store.put(bitmap);
        });

        transaction.oncomplete = function () {
            console.log('All meme sound bitmaps have been saved to the database.');
        };

        transaction.onerror = function (event) {
            console.error('Transaction error:', transaction.error);
        };
    };

    request.onerror = function (event) {
        console.error('Database error:', request.error);
    };
}