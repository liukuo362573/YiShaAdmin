 rm -rf publish

 find yishaadmin -name '*.dll' -type f -print -exec rm -rf {} \;
 find yishaadmin -name '*.pdb' -type f -print -exec rm -rf {} \;
 find yishaadminapi -name '*.dll' -type f -print -exec rm -rf {} \;
 find yishaadminapi -name '*.pdb' -type f -print -exec rm -rf {} \;

 7za x Publish.7z  -opublish

 \cp -rf publish/YiShaAdmin/* yishaadmin
 \cp -rf publish/YiShaAdminApi/* yishaadminapi

 supervisorctl restart YiShaAdmin
 supervisorctl restart YiShaAdminApi