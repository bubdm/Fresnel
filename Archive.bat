@ set D=%date:~-4,4%%date:~-7,2%%date:~-10,2%%
@ set hh=%time:~0,2%
@ if "%time:~0,1%"==" " set hh=0%hh:~1,1%
@ set T=%hh%%time:~3,2%%time:~6,2%%

@ set SOURCE_FOLDER=.
@ set ARCHIVE=Fresnel_V0_%D%_%T%.7z

@ rem Package it:
"C:\Program Files\7-Zip\7z.exe" a -t7z -r -v100M "%ARCHIVE%" "%SOURCE_FOLDER%\*" -x@ArchiveExcludes.txt

