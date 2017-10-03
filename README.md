### Quick start

#### Step One

```bash

# clone our repo
# --depth 1 removes all but one .git commit history
git clone --depth 1 https://github.com/behdad088/BookStore.git

# change directory to this repo
cd BookStore, then run the book.sln file and run the Web API application


```

#### Step Two
```bash
**Make sure you have Node version >= 4.0 and NPM >= 3**
> Clone/Download the repo then edit `app.ts` inside [`/src/app/app.ts`](/src/app/app.ts)

# change directory to this repo
cd bookStore.Client

# add required global libraries
npm install typings webpack-dev-server rimraf webpack -g

# install the repo with npm
npm install

# start the server
npm start

# use Hot Module Replacement
npm run server:dev:hmr

Note: Url to frontend application is http://localhost:3000
```
