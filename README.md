LongPath
========

A drop-in library to support long paths in .NET

Supporting files and directories with a long path are fairly easily with Windows.  Unforutnately, other aspects of Windows haven't supported long paths in their entirely.  The file system, for example, supports long paths quite well; but other things like Command Prompt and Explorer don't.  This makes it hard to entire support long paths.

This has been a bit tricky in .NET.  Several attemps like [longpaths.codeplex.com](http://longpaths.codeplex.com/) (which a more up to date version has made its way into .NET in classes like [LongPath](http://referencesource.microsoft.com/#mscorlib/system/io/longpath.cs) [LongPathFile](http://referencesource.microsoft.com/#mscorlib/system/io/longpath.cs#734b3020e7ff04fe#references) and [LongPathDirectory](http://referencesource.microsoft.com/#mscorlib/system/io/longpath.cs#ed4ae27b0c89bf61#references).  But, these libraries did not seem to support the entire original API (`Path`, `File`, `Directory`) and not all file-related APSs (including `FileInfo`, `DirectoryInfo`, `FileSystemInfo`).  LongPath attempts to rectify that.

LongPath originally started as a fork of LongPaths on Codeplex; but after initial usage it was clear that much more work was involved to better support long paths.  So, I drastically expanded the API scope to include `FileInfo`, `DirectoryInfo`, `FileSystemInfo` to get 100% API coverage supporting long paths.  (with one caveat: `Directory.SetCurrentDirectory`, Windows does not support long paths for a current directory).

LongPaths allows your code to support long paths by providing a drop-in replacement for the following `System.IO` types: `FileInfo`, `DirectoryInfo`, `FileSystemInfo`, `FileInfo`, `DirectoryInfo`, `FileSystemInfo`

Usage
=====

**TBD**

Caveats
=======

**TBD**

How long paths can be created
=============================

Long paths can be created *accidentally* in Windows making them very hard to process.

**TBD**