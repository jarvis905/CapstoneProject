from flask import Flask, jsonify, request
import pyodbc
import pandas as pd
from surprise import SVD, Reader, Dataset

app = Flask(__name__)

# Connection variables
server = 'capstonedb.czjqafpaejcf.ca-central-1.rds.amazonaws.com,1960'
database = 'Capstonedb'
username = 'admin'
password = 'capstone123Yekt!'

# Load data into a Pandas DataFrame
def load_data():
    try:
        # Connection string
        cnxn = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};SERVER=' +
                              server+';DATABASE='+database+';UID='+username+';PWD=' + password)

        # Fetch data into a DataFrame
        query = "SELECT * FROM Reviews;"
        df = pd.read_sql(query, cnxn)

        # Close the database connection
        cnxn.close()

        return df

    except Exception as e:
        print(f"Error: {e}")
        return pd.DataFrame()  # Return an empty DataFrame in case of an error

# Collaborative Filtering with Surprise
def get_collaborative_recommendations(user_id):
    df_rating = load_data()

    # Create Surprise Reader and Dataset
    reader = Reader(rating_scale=(1, 5))
    data = Dataset.load_from_df(df_rating[['UserId', 'MovieId', 'UserRate']], reader)

    # Train the collaborative filtering model
    svd = SVD()
    trainset = data.build_full_trainset()
    svd.fit(trainset)

    # Get movie recommendations for the specified user
    movies_list = df_rating['MovieId'].unique()
    unrated_movies = list(set(movies_list) - set(df_rating[df_rating['UserId'] == user_id]['MovieId']))

    recommendations = get_user_recommendations(svd, user_id, unrated_movies)

    return recommendations

def get_user_recommendations(algo, user_id, movies_list, n=5):
    user_ratings = [(user_id, movie, 3.0) for movie in movies_list]  # Default rating for unrated movies
    predictions = [algo.predict(user_id, movie) for _, movie, _ in user_ratings]
    recommendations = sorted(predictions, key=lambda x: x.est, reverse=True)[:n]
    return [str(recommendation.iid) for recommendation in recommendations]  # Convert movie IDs to strings

# API endpoint to get collaborative recommendations for a given user
@app.route('/api/get_collaborative_recommendations', methods=['GET'])
def api_get_collaborative_recommendations():
    try:
        user_id = request.args.get('user_id')
        recommendations = get_collaborative_recommendations(user_id)
        return jsonify({'user_id': user_id, 'collaborative_recommendations': recommendations})

    except Exception as e:
        return jsonify({'error': str(e)})

if __name__ == '__main__':
    app.run(debug=True)
