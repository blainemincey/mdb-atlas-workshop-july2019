#!/usr/bin/env python3
from pymongo import MongoClient

####
# Indicate start
####
print("============================")
print("  Starting my_python_app    ")
print("============================")
print('\n')


####
# Main start function
####
def main():
    # MongoDB Connection
    mongo_client = MongoClient(MONGODB_URL)
    db = mongo_client[DATABASE]
    patients_collection = db[COLLECTION]

    # Define the query
    # Comment/uncomment one of the following query variables to either enable a filter or return all
    # Projection returns a subset of fields

    query = { 'year' : {'$gte' : 2015}, 'cast' : 'Chris Pratt' }
    #query = {}

    projection = {'_id': 0, 'title' : 1, 'cast' : 1}

    for doc in patients_collection.find(query, projection):
        print(doc)


####
# Constants
####
MONGODB_URL = '### Your MongoDB Atlas URL ###'
DATABASE = 'sample_mflix'
COLLECTION = 'movies'


####
# Main
####
if __name__ == '__main__':
    main()


####
# Indicate End
####
print('\n')
print("============================")
print("  Ending my_python_app      ")
print("============================")