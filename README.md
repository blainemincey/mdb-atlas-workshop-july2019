# mdb-atlas-workshop-july2019
This repository contains a guide on getting started with MongoDB.  Specifically, this is
a step-by-step tutorial/workshop incorporating the use of [MongoDB Atlas](https://www.mongodb.com/cloud/atlas),
[MongoDB Compass](https://www.mongodb.com/products/compass), [MongoDB Stitch](https://www.mongodb.com/cloud/stitch), 
and [MongoDB Charts](https://www.mongodb.com/products/charts).  
The data that will be utilized in this workshop
will be one of the sample data sets that can be loaded into MongoDB Atlas via the
click of a button.

## Introduction to MongoDB and MongoDB Atlas
### Hands-on Workshop
#### Overview
This hands-on workshop is designed to get you familiar with all general aspects of
MongoDB and MongoDB Atlas.  However, the general focus will be on utilizing the capabilities
of MongoDB Atlas.  The exercises will include basic query capabilities, introduction
to the aggregation framework, query optimization, and schema validation through the
use of MongoDB Compass.  The exercises will then transition into building applications
using MongoDB Stitch through the use of [Stitch Functions](https://docs.mongodb.com/stitch/functions/) 
and [Stitch Triggers](https://docs.mongodb.com/stitch/triggers/).  Finally, the workshop
will conclude with a set of exercises to introduce the user to MongoDB Charts.

### Required Prerequisites
To successfully complete this workshop, the following steps should be completed:
* To use MongoDB Atlas, you **must** be able to make outgoing requests from your computer
to MongoDB Atlas services which will be running on port 27017.  Please confirm that port
27017 is not blocked by checking with [PortQuiz](http://portquiz.net:27017/).

* Complete [Getting Started with MongoDB Atlas](https://docs.atlas.mongodb.com/getting-started/). This involves creating an Atlas account, creating your first
free tier cluster, and setting up the appropriate security credentials

* Load Sample data into your cluster by following the [Insert Data into Your Cluster](https://docs.atlas.mongodb.com/getting-started/#insert-data-into-your-cluster)
section of the Getting Started Guide.

* Download/Install [MongoDB Compass](https://www.mongodb.com/products/compass).

* Verify the sample databases/collections have been successfully loaded into your
MongoDB Atlas cluster by using either MongoDB Compass or the [Data Explorer](https://docs.atlas.mongodb.com/data-explorer/index.html)
tool that is part of the MongoDB Atlas User Interface.

#### Important Note:
As you walk through the exercises, you will have the option of interfacing with the data
that is stored in MongoDB Atlas through a number of tools.  Several of the MongoDB Stitch
exercises will be utilizing a microservices style architecture by making calls via
a REST API.  For these examples you can use [curl](https://curl.haxx.se/download.html) or
[Postman](https://www.getpostman.com/).

### MongoDB Compass Lab
#### Lab 1 - Connect to your cluster in MongoDB Atlas
If you have completed the steps above, we can now copy the connection string and
connect Compass to our cluster.  Within the Atlas UI, in the Clusters view, click
the *Connect* button.  This will bring up a pop-up window where you can select how
to connect to your Atlas Cluster.  Select the option *Connect with MongoDB Compass*.
The image below should provide an idea on what you should see:

![](img/connectcompass.jpg)  

Be sure to select that you already have Compass and select the latest version which is
indicated by *1.12 or later*.  Then, copy your connection string.  When you now open
or point to MongoDB Compass, it will read your copy buffer and you can easily paste
this into MongoDB Compass.  All you should have to do is enter your password you used
when you first created a MongoDB Atlas database user or Atlas admin user.  Be sure
to select to save this as a favorite as this makes it much quicker to open the cluster
up after closing Compass.

Once you are in Compass and connected to Atlas, you should see a view simiar to that below:

![](img/compassconnected.jpg)

#### Lab 2 - Browse documents
Click the *sample_mflix* database to open up a series of collections.  Then, select
the *theaters* collection. It should have a list of the documents in this collection
similar to that below:

![](img/browsedocs.jpg)

There is simply an _id field, a theaterId field, and a location object.  Wait, did we
just store and object in the database?  We can expand the location field/object and
see that it is storing an address and embedding yet another object for its geolocation.
Working with data in this way, it is much easier than having to flatten out multiple
tables from a relational/tabular model into a single object.

#### Lab 3 - Analyze the Schema
Analyze the schema???  Wait, I thought that MongoDB was a NoSQL database and was considered
to be schema-less?  While that is technically true, no dataset would be of any use
without the notion or concept of a schema.  Although MongoDB does not enforce a schema,
your collections of documents will still always have one.  The key difference with MongoDB
is that the schema can be flexible/polymorphic.

Within MongoDB Compass, select the *Schema* tab and click *Analyze Schema*.  Compass
will sample the documents in the collection to derive a schema.  In addition to providing,
field names and types, Compass will also provide a summary of the data values and their
distribution.  

After you click *Analyze Schema*, if you look at the *location* field, you will notice
that it contains a *geo* field with GeoJSON coordinates, i.e. a longitude and latitude coordinate.
You can drill down on the map as it builds the query for you.  It should look similar
to the image below:

![](img/compassmap.jpg)  

#### Lab 4 - Query Data with MongoDB Compass (CRUD Operations)
Simply copy the code block for each section below and paste within the filter
dialog in MongoDB Compass as indicated below:

![](img/compass-filterblock.jpg)  

##### Find Operations  

* Simple filter (specific name in sample_mflix.comments collection)
```
{ name : "Ned Stark" }
```

* Simple filter with query operators (email that ends in fakegmail.com and comment made since 2017 in sample_flix.comments)
```
{ email: /fakegmail.com$/ , date : {$gte : ISODate('2017-01-01')}}  
```

* Query sub-document/array  (movies with more than 10 wins in sample_mflix.movies)
```
{"awards.wins" : {$gt : 10}}
```

* Query using the $in operator (Movies in Arabic or Welsh in sample_mflix.movies)
```
{languages : {$in : ["Arabic", "Welsh"]}}
```

* Only movies in Arabic
```
{languages : ["Arabic"] }
``` 

* Movies in BOTH Arabic and Spanish
```
{languages : { $all : ["Arabic", "Spanish"]}}
```

* Movies with IMDB Rating > 8.0 and PG Rating
```
{"imdb.rating": {$gt : 8.0 }, rated : "PG"}
```

* Movies with Title starting with "Dr. Strangelove"
```
{ title : /^Dr. Strangelove/ }
```

##### Update, Delete, Clone Operations
Find a document in any collection and choose to update a field or fields.  In fact,
you can select to add a field that does not exist.  Next, clone a document.  Then,
delete the cloned document.  MongoDB Compass provides all the CRUD controls you
will need.  Below is a sample image displaying where this capability is:

![](img/copyclonedelete.jpg)

#### Lab 5 - Create indexes to improve efficiency of queries - Part 1
Indexes support the efficient execution of queries in MongoDB.  Without indexes,
MongoDB must perform a *collection scan*, i.e., scan every document in a collection
to select those documents that match the query statement.  If an appropriate
index exists for a query, MongoDB can use the index to limit the number of documents
it must inspect.

In this lab, we will perform a search on a field, use the explain plan to determine
if it could be improved with an index, and create the index...all from within MongoDB Compass.

For this lab, we will execute a query to find all movies with at least Drama as a genre.

In the query box, enter:
```
{genres : "Drama" }
```

Then, click the Explain Plan tab as below:

![](img/explainplan1.jpg)  

Click the *Execute Explain* button in the middle of the GUI and review the output.

![](img/explain2.jpg)  

Considering this is a relatively small data size, an index may not immediately improve
performance.  In our results, it indicates a *collection scan* took place with our filter.
It also indicates that it took 7 milliseconds to execute.

Click on the indexes tab.  We will create an index on *genres*.  Find the field in the
dropdown and select 'asc' as the index type.  Then, click the create button as below:

![](img/createindex.jpg)  

After creating the index, go back to your Explain Plan tab and *Execute Explain* once more.
This time, you should see that your index was hit and instead of a collection scan, your
filter performed an index scan.  Your results should look similar to below:

![](img/createindex2.jpg)  

#### Indexes - Part 2
Now, try the following query:
```
{released : {$gte : ISODate('2010-01-01')}, rated : 'R'}
```
**Be sure to sort this query with the following:**
```
{"imdb.rating" : 1 }
```

After executing the query, try to run an *Explain Plan* on it.  It should look
like the following:

![](img/indexpart2.jpg)  

Any ideas or suggestions on a possible index?  Based on our query, the first
thought may be to create an ascending index on released, rated, and imdb.rating.
You can try this in fact and see the results.  It should resemble this below:

![](img/index2part1.jpg)  

However, the general rule of thumb is to remember: Equality, Sort, and Range. Consider
that we are matching exactly on rated.  Then, we sort on the imdb.rating.  Finally,
our range is the released date.  Delete the previously created index.  Let's try this now by creating an ascending index
in the order of rated, imdb.rating, and the released fields.  It should look like below:

![](img/newidx.jpg)  

Go back to the *Explain Plan* tab and let's see the new results:

![](img/mynewidxresult.jpg)  

This is a great rule of thumb:  Equality, Sort, Range!

#### Lab 6 - Aggregation Framework


### MongoDB Stitch Labs
#### Lab 1 - Create a MongoDB Stitch Application

#### Lab 2 - Create a REST API (i.e. microservices)
##### GET Method
##### POST Method

#### Lab 3 - Create a Stitch Trigger

#### Lab 4 - Query Anywhere with Stitch (Static HTML)

#### Lab 5 - MongoDB Static Hosting

### MongoDB Charts
#### Lab 1 - Create a MongoDB Chart

#### Lab 2 - Embedding MongoDB Charts

### Controlling Access to Data using Rules with MongoDB Stitch
https://github.com/blainemincey/stitch-rules-mdbw2019


### Code Samples
#### Python
#### Node.js
#### C#







