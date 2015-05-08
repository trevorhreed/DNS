Grunt 0.1.13
============

Grunt is the JavaScript task runner, which main purpose is to automate repetitive tasks like minification, compilation, unit testing, linting, etc.


Installation overview
---------------------

Grunt cmd is installed into .bin dir in your project, along with Node.js cmd. NuGet rules are
the same: everything deployed by NuGet, so use package restore and ignore packages in VCS.


Automation
----------

Use ".bin\grunt" command to run Grunt in your build scripts. For example, here is a simple
MsBuild target to run "build" Grunt task from the project Gruntfile.js:

<Target Name="GruntBuild">
  <Exec Command=".bin\grunt build" />
</Target>


Daily usage
-----------

Because ".bin" was added to your PATH, you should be able to run Grunt directly in the
command prompt from the project dir. For example, if "MySite.Web" is a project dir:

D:\Projects\MySite\MySite.Web> grunt watch

Note: add ".bin" to the PATH manually for other developers in your team.
Note: if PATH was changed, restart your command prompt to refresh environment variables.


Docs
----

Read Grunt documentation at http://gruntjs.com/getting-started


------------------------------------------------------
© 2014 Grunt Team