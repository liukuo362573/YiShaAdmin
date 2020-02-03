 rm -rf publish

 find admin -name '*.dll' -type f -print -exec rm -rf {} \;
 find admin -name '*.pdb' -type f -print -exec rm -rf {} \;
 find api -name '*.dll' -type f -print -exec rm -rf {} \;
 find api -name '*.pdb' -type f -print -exec rm -rf {} \;

 7za x Publish.7z  -opublish

 \cp -rf publish/Admin/* admin
 \cp -rf publish/Api/* api

 supervisorctl restart YiShaAdmin  # supervisord.d配置的ini文件的program
 supervisorctl restart YiShaApi    # supervisord.d配置的ini文件的program