%% configration
ShowDetail = 1;
fw = fopen('errlog.md','w');
ff = fopen('err.md','w');


%% Core_II_ICD.gcc

if ~exist('Core_II_ICD.gcc','file')==0
    filename = 'Core_II_ICD.gcc';
    disp([char(10) 'Handling ' filename ' ......' char(10)]);
    fprintf(fw,'\n# Handling %s ...... \n\n',filename);
    fprintf(ff,'\n# Handling %s ...... \n\n',filename);
   
   data=textread(filename,'%[^\n]');
   n=1;
   
   for i=1:length(data)
       k = strfind(data(i), 'Error :');
       if k{1}==1
%            disp([num2str(i) ':' num2str(k{1})]);
           disp(['No.' num2str(n) ': ' data{i}]);
           fprintf(fw,'-  %s\n',data{i});
           fprintf(ff,'-  %s\n',data{i});
           j = i+1;
           if ShowDetail == 1 && length(strfind(data{j}, 'Info : ')) + length(strfind(data{j}, 'Warning : ')) + length(strfind(data{j}, 'Error : ')) == 0
               fprintf(fw,'```\n');
               while length(strfind(data{j}, 'Info : ')) + length(strfind(data{j}, 'Warning : ')) + length(strfind(data{j}, 'Error : ')) == 0
                   disp(data{j});
                    fprintf(fw,'%s\n',data{j});
                   j=j+1;
               end
               fprintf(fw,'```\n');
           end
           n=n+1;
       end
   end
else
    disp([char(10) 'Core_II_ICD.gcc FILE DO NOT EXIST.'])
    fprintf(fw,'\n# Core_II_ICD.gcc FILE DO NOT EXIST.\n');
    fprintf(ff,'\n# Core_II_ICD.gcc FILE DO NOT EXIST.\n');
end

%% c2st.err

if ~exist('c2st.err','file')==0
    filename = 'c2st.err';
   disp([char(10) 'Handling ' filename ' ......' char(10)]);
   fprintf(fw,'\n# Handling %s ...... \n\n',filename);
   fprintf(ff,'\n# Handling %s ...... \n\n',filename);
   
   data=textread(filename,'%[^\n]');
   n=1;
   
   for i=1:length(data)
       k = strfind(data(i), 'Error :');
       if k{1}==1
%            disp([num2str(i) ':' num2str(k{1})]);
           disp(['No.' num2str(n) ': ' data{i}]);
           fprintf(fw,'No.%d: %s\n',n,data{i});
           fprintf(ff,'No.%d: %s\n',n,data{i});
           j = i+1;
           if ShowDetail == 1
               while length(strfind(data{j}, 'Info : ')) + length(strfind(data{j}, 'Warning : ')) + length(strfind(data{j}, 'Error : ')) == 0
                   disp(data{j});
                    fprintf(fw,'%s,\n',data{j});
                   j=j+1;
               end
           end
           n=n+1;
       end
   end
else
    disp([char(10) 'c2st.err FILE DO NOT EXIST.'])
    fprintf(ff,'\n# c2st.err FILE DO NOT EXIST.\n');
    fprintf(fw,'\n# c2st.err FILE DO NOT EXIST.\n');
end

%% ecfg.err

if ~exist('ecfg.err','file')==0
    filename = 'ecfg.err';
   disp([char(10) 'Handling ' filename ' ......' char(10)]);
   fprintf(fw,'\n# Handling %s ...... \n\n',filename);
   fprintf(ff,'\n# Handling %s ...... \n\n',filename);
   
   data=textread(filename,'%[^\n]');
   n=1;
   
   fprintf(fw,'## ERROR:\n%s',data{end});
   fprintf(ff,'## ERROR:\n%s',data{end});
   
   fprintf(fw,'\n## EXCLUDING:\n');
   fprintf(ff,'\n## EXCLUDING:\n');
   disp(data{end});
   disp(char(10));
   disp('EXCLUDING:');
   
   for i=1:length(data)
       k = strfind(data(i), 'EXCLUDING');
       if k{1}==10
%            disp([num2str(i) ':' num2str(k{1})]);
           disp(['No.' num2str(n) ': ' data{i}]);
           fprintf(fw,'- %s\n',data{i});
           fprintf(ff,'- %s\n',data{i});
           j = i+1;
           if length(strfind(data{j}, 'INCLUDING')) == 0
               fprintf(ff,'```\n');
               fprintf(fw,'```\n');
               while length(strfind(data{j}, 'INCLUDING')) == 0
                   disp(data{j});
                   fprintf(fw,'%s\n',data{j});
                   fprintf(ff,'%s\n',data{j});
                   j=j+1;
               end
               fprintf(ff,'```\n');
               fprintf(fw,'```\n');
           end
           n=n+1;
       end
   end
else
    disp([char(10) 'ecfg.err FILE DO NOT EXIST.'])
    fprintf(ff,'\n# ecfg.err FILE DO NOT EXIST.\n');
    fprintf(fw,'\n# ecfg.err FILE DO NOT EXIST.\n');
end

%% fclose
fclose(fw);
fclose(ff);