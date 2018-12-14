require 'fileutils'

namespace :build do
	task :publish do
		dest = '.';
		FileUtils.cp_r 'HushNow/bin/Release', dest
		puts 'Publish successful'
	end
end
