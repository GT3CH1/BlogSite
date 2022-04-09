# Gavin's Blog Site


A blog application written in C# and ASP.NET Core.
## Live demo
[blog.peasenet.com](https://blog.peasenet.com)
## Features

- Easy to use and deploy.
- Text editing is done in the browser via TinyMCE.
- Blog posts are stored in a SQL database.
- Image upload.
- [Docker support](#docker-support).
- A basic search engine to search through titles and content.

## Building

### Installing ASP.NET Core

#### Linux

I developed this application using Ubuntu 20.04 via WSL2. I folowed the instructions
listed [here](https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu) to install ASP.NET Core on Ubuntu
20.04. Please note that other versions of linux are supported by ASP.NET Core, and thus this application will work on
other linux distros.

#### Windows & MacOS

You can install ASP.NET Core [here](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and follow the instructions
to install dotnet and asp.net on your computer.

#### Building the application (binary)

Building the application is trivial. I used the command line to build the application.

```bash
dotnet build
```

This will build the application and place binaries in the BlogSite/bin/Debug/net6.0 directory.

#### Running the application

Running the application is also trivial. Just navigate to the BlogSite directory and run the application.

```bash
cd BlogSite/bin/Debug/net6.0
dotnet BlogSite.dll
```

This will create a few files on startup - app.db, posts.db, and a "Media" directory.

#### Adding an admin user

At this present moment, there is two roles - an Admin role which allows users to post, and a default role. To create an
admin user, first create a user using the website itself (naviage to the login page, create a user there.)
After this is done, you can add the email that was used for the account to the "Administartors" list in the file
`appsettings.json`. By default, the user with the email `email@email.com` will be granted administrator rights
on the next startup of the program. You will need to reboot the application after adding an administrator.
```json
...
"Administrators": [
      "email@email.com"
    ]
...
```
#### Docker support
You can build the application using Docker.
```bash
docker build -t blogsite -f BlogSite/Dockerfile .
```
After building the image, you can run the application. It is recommended to create
two directories somewhere for the application to store data - one for the databases, and one for the media.
#### Creating the directories
```bash
mkdir ~/blogsite-media
mkdir ~/blogsite-db
```

##### Running the application via docker
```bash
docker run -v ~/blogsite-db:/db -v ~/blogsite-media:/app/Media -p 80:80 -d --name blogsite blogsite
```
#### Using a prebuilt docker image.
Each commit to the master branch will have a docker image automatically created (rolling release style).
You can use the current image by running the following command.
```bash
docker pull gcpease/blogsite:latest
```
After running this command, you can run the application.
```bash
 docker run -v ~/blogsite-db:/db -v ~/blogsite-media:/app/Media -dp 7160:80 --name blogsite gcpease/blogsite:latest
 ```
#### Updating the prebuild docker image.
To update the docker image, run the following commands.
```bash
docker stop blogsite
docker rm blogsite
docker pull gcpease/blogsite:latest
docker run -v ~/blogsite-db:/db -v ~/blogsite-media:/app/Media -dp 7160:80 --name blogsite gcpease/blogsite:latest
```
## Future Features
- Better role management.
- A draft system.
- Comments.
- Tagging.
## Things to work on
- [ ] XSS prevention
- [ ] Post content and title sanitization.
- [ ] A better way to store/render images/media.
## Background

### Why would I develop another CMS?

If you have been following my development for a few years now, you might have notice dthat I have written a new blogging
system about once a year. With each iteration, I have added more features and have tinkered with a different language.
My first CMS originally started out from a static HTML page that was generated by a bash script, which evolved to a
system that used MySQL and PHP. This system worked, but there were a lot of flaws with using PHP, and flaws within the
software itself. I decided to write a new system that was more flexible and easier to use, as well as more secure. This
led to the creation of this CMS which is powered by ASP.NET Core, C#, and Sqlite. This will hopefully be my last
iteration of a blog system, and I hope to continue to add more features to it. This CMS has proven to be a fun
challenge, as well as a practical application of knowledge I have gained from classes I have taken.

### Why C#?

I have always been interested in C#, and I have always had a strong interest in ASP.NET Core. I have used C# in the past
for some school assignments and enjoyed it. I've wanted to learn how to use ASP.NET Core and the MVC architecture for
quite some time, so this made C# an excellent choice.



