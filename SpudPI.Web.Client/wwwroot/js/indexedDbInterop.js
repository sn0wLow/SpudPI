function openDatabase() {
    return new Promise((resolve, reject) => {
        const dbName = 'MemeSoundBitmapDatabase';
        const storeName = 'memeSoundBitmaps';
        const version = 1;
        const request = indexedDB.open(dbName, version);

        request.onupgradeneeded = function (event) {
            const db = event.target.result;
            if (!db.objectStoreNames.contains(storeName)) {
                db.createObjectStore(storeName, { keyPath: 'memeId' });
            }
        };

        request.onsuccess = function (event) {
            resolve(event.target.result);
        };

        request.onerror = function (event) {
            reject('IndexedDB opening error: ' + request.error);
        };
    });
}

async function saveMemeSoundBitmapsToIndexedDB(jsonData) {
    const memeSoundBitmaps = JSON.parse(jsonData);
    const db = await openDatabase();
    const transaction = db.transaction(['memeSoundBitmaps'], 'readwrite');
    const store = transaction.objectStore('memeSoundBitmaps');

    memeSoundBitmaps.forEach(bitmap => {
        store.put(bitmap);
    });

    return new Promise((resolve, reject) => {
        transaction.oncomplete = () => resolve('All meme sound bitmaps have been saved to the database.');
        transaction.onerror = () => reject(new Error(transaction.error));
    });
}

async function loadMemeSoundBitmapsFromIndexedDB(keys) {
    const db = await openDatabase();
    return new Promise((resolve, reject) => {
        const transaction = db.transaction(['memeSoundBitmaps'], 'readonly');
        const store = transaction.objectStore('memeSoundBitmaps');
        const memes = [];

        keys.forEach(key => {
            const request = store.get(key);
            request.onsuccess = () => {
                if (request.result) memes.push(request.result);
            };

            /*request.onerror = () => reject('Failed to fetch meme sound bitmap: ' + request.error);*/
        });


        transaction.oncomplete = () => resolve(memes);
        transaction.onerror = () => reject(new Error(transaction.error));
    });
}

//async function loadMemeSoundBitmapsFromIndexedDB(keys) {
//    const db = await openDatabase();
//    return new Promise((resolve, reject) => {
//        const transaction = db.transaction(['memeSoundBitmaps'], 'readonly');
//        const store = transaction.objectStore('memeSoundBitmaps');
//        const memes = [];

//        // Keep track of how many requests have been processed
//        let processedRequests = 0;

//        keys.forEach(key => {
//            const request = store.get(key);
//            request.onsuccess = () => {
//                if (request.result) {
//                    memes.push(request.result);
//                }
//                // Increment the count of processed requests
//                processedRequests++;
//                // Check if all requests have been processed
//                if (processedRequests === keys.length) {
//                    resolve(memes); // Resolve the promise with the collected memes
//                }
//            };

//            request.onerror = () => {
//                // Even if there's an error (e.g., permission issue, not missing ID), increment processed count
//                processedRequests++;
//                // Check if this was the last pending request
//                if (processedRequests === keys.length) {
//                    resolve(memes); // Resolve with whatever was collected
//                }
//            };
//        });

//        transaction.onerror = () => reject(new Error('Transaction error: ' + transaction.error));
//    });
//}