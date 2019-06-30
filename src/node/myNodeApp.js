const MongoClient = require('mongodb').MongoClient;
const assert = require('assert');

// Connection URL
const dbUrl = '### YOUR MONGODB Atlas URL ###';

// Database Name
const databaseName = 'sample_mflix';

// find documents
const findDocuments = function (db, callback) {
    const collection = db.collection('movies');

    let query = { 'year' : {'$gte' : 2015}, 'cast' : 'Chris Pratt' };
    // let query = {};

    let projection = {'_id': 0, 'title' : 1, 'cast' : 1};

    let cursor = collection.find(query);
    cursor.project(projection);

    cursor.forEach(
        function (doc) {
            console.log(doc);
        }, function (err, result) {
            assert.equal(err, null);
            callback(result);
        }
    )

}

// Connect to the database
MongoClient.connect(dbUrl, {useNewUrlParser: true}, function (err, client) {
    assert.equal(null, err);
    console.log("Connected successfully to MongoDB!");

    const db = client.db(databaseName);


    findDocuments(db, function () {
        client.close();
    })

});